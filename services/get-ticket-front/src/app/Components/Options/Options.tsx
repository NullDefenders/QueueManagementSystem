import { OptionButton } from "./OptionButton/OptionButton";

import styles from "./Options.module.scss";

interface OptionsProps {
  list: string[];
  onClick: (value: string) => void;
}

export const Options = (props: OptionsProps) => {
  return (
    <ul className={styles.options}>
      {props.list.map((item) => {
        return (
          <OptionButton
            key={item}
            onClick={() => props.onClick(item)}
            title={item}
          />
        );
      })}
    </ul>
  );
};
