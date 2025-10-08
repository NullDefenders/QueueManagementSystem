import { Options } from "../../Options/Options";

import styles from "./StepCategories.module.scss";

interface StepCategoriesProps {
  categories: string[];
  onCategoryClick: (category: string) => void;
}

export const StepCategories = (props: StepCategoriesProps) => {
  return (
    <div className={styles.stepCategories}>
      <h1 className={styles.stepCategories__title}>Выберите категорию</h1>
      <Options list={props.categories} onClick={props.onCategoryClick} />
      <label className={styles.stepCategories__inputLabelContainer}>
        <span className={styles.stepCategories__inputLabel}>
          или введите номер бронирования
        </span>
        <input type="text" className={styles.stepCategories__input}></input>
      </label>
    </div>
  );
};
