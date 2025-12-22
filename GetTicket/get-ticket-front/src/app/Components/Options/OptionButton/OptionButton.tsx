import styles from "./OptionButton.module.scss";

interface OptionButtonProps {
  onClick: () => void;
  title: string;
  desc?: string;
}

export const OptionButton = (props: OptionButtonProps) => {
  return (
    <li className={styles.optionButtonContainer}>
      <button className={styles.optionButton} onClick={props.onClick}>
        <span className={styles.optionButton__title}>{props.title}</span>
        {props.desc && (
          <span className={styles.optionButton__desc}>{props.desc}</span>
        )}
      </button>
    </li>
  );
};
