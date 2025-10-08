import Image from "next/image";

import { Steps } from "../../types";

import styles from "./Header.module.scss";

interface HeaderProps {
  setStep: (step: Steps) => void;
}

export const Header = (props: HeaderProps) => {
  return (
    <header className={styles.header}>
      <Image
        className={styles.header__logo}
        src="/queue-logo.jpg"
        height={208}
        width={400}
        alt="logo"
        onClick={() => props.setStep(Steps.Departments)}
      />
    </header>
  );
};
