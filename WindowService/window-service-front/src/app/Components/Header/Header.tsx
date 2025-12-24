import Image from "next/image";

import styles from "./Header.module.scss";

export const Header = () => {
  return (
    <header className={styles.header}>
      <Image
        className={styles.header__logo}
        src="/logo-queue-1.png"
        height={208}
        width={400}
        alt="logo"
      />
    </header>
  );
};
