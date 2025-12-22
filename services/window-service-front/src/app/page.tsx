import Image from "next/image";

import { Header } from "./Components/Header/Header";
import { OperatorWindow } from "./Components/OperatorWindow/OperatorWindow";

import styles from "./page.module.scss";

export default function Home() {
  return (
    <div className={styles.page}>
      <Header />
      <OperatorWindow />
    </div>
  );
}
