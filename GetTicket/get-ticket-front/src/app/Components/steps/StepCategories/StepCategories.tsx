import { useEffect, useState } from "react";
import { Options } from "../../Options/Options";

import styles from "./StepCategories.module.scss";

interface StepCategoriesProps {
  categories: string[];
  onCategoryClick: (category: string) => void;
  onServcieNameClick: (serviceName: string, pendingTime: Date) => void;
}

export const StepCategories = (props: StepCategoriesProps) => {
  const [inputValue, setInputValue] = useState("");
  const [inputError, setInputError] = useState("");

  useEffect(() => {
    if (inputValue.length === 5) {
      fetch(`https://localhost:44345/record/${inputValue}`)
        .then((response) => response.json())
        .then((data) => {
          console.log(new Date(data.recordTime));
          props.onServcieNameClick(data.serviceName, new Date(data.recordTime));
        })
        .catch(() => {
          setInputError("Нет такого кода бронирования");
        })
        .finally(() => {
          setInputValue("");
        });
    }
  }, [inputValue]);

  return (
    <div className={styles.stepCategories}>
      <h1 className={styles.stepCategories__title}>Выберите категорию</h1>
      <Options list={props.categories} onClick={props.onCategoryClick} />
      <label className={styles.stepCategories__inputLabelContainer}>
        <span className={styles.stepCategories__inputLabel}>
          или введите номер бронирования
        </span>
        <div className={styles.stepCategories__inputContainer}>
          <input
            type="text"
            className={styles.stepCategories__input}
            value={inputValue}
            onChange={(e) => {
              setInputError("");
              setInputValue(e.target.value);
            }}
            maxLength={5}
          ></input>
          {inputError && (
            <span className={styles.stepCategories__inputError}>
              {inputError}
            </span>
          )}
        </div>
      </label>
    </div>
  );
};
