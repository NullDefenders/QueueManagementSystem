"use client";

import { useState } from "react";
import Image from "next/image";
import clsx from "clsx";

import styles from "./OperatorWindow.module.scss";

export const OperatorWindow = () => {
  const defaultWindow = "WINDOW SYSTEM";
  const [currentWindow, setCurrentWindow] = useState(defaultWindow);

  return (
    <main className={styles.operatorWindow}>
      <div className={styles.windowContainer}>
        <Image src="/window.png" height={430} width={800} alt="logo" />
        <div
          className={clsx(
            styles.currentWindow,
            defaultWindow === currentWindow && styles.currentWindow_default
          )}
        >
          {currentWindow}
        </div>
      </div>
      <div className={styles.windowButtonsContainer}>
        {["1", "2", "3", "4", "5"].map((num) => {
          return (
            <button
              key={num}
              className={styles.windowButton}
              onClick={() => setCurrentWindow(num)}
            >
              {num}
            </button>
          );
        })}
      </div>
      <button className={clsx(styles.button, styles.button_free)}>
        Свободен
      </button>
      <button className={clsx(styles.button, styles.button_busy)}>Занят</button>
    </main>
  );
};
