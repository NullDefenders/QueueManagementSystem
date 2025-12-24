"use client";

import { useState } from "react";
import Image from "next/image";
import clsx from "clsx";

import styles from "./OperatorWindow.module.scss";

export const OperatorWindow = () => {
  const defaultWindow = "WINDOW SYSTEM";
  const [currentWindow, setCurrentWindow] = useState(defaultWindow);
  const [openedWindows, setOpenedWindows] = useState<Set<string>>(new Set());

  const onServcieNameClick = (windowNumber: string, statusStr: string) => {
    fetch("http://localhost:8084/api/Window", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        windowNumber,
        status: statusStr === "free" ? 0 : 1,
      }),
    }).then(() => {
      setOpenedWindows((prevOpenedWindows) => {
        const newSet = new Set(prevOpenedWindows);

        if (statusStr === "free") {
          newSet.add(windowNumber);
        } else {
          newSet.delete(windowNumber);
        }

        return newSet;
      });
    });
  };

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
              className={clsx(
                styles.windowButton,
                openedWindows.has(num) && styles.windowButton_open
              )}
              onClick={() => {
                setCurrentWindow(num);
              }}
            >
              {num}
            </button>
          );
        })}
      </div>
      <button
        className={clsx(styles.button, styles.button_free)}
        onClick={() => {
          onServcieNameClick(currentWindow, "free");
        }}
      >
        Свободен
      </button>
      <button
        className={clsx(styles.button, styles.button_busy)}
        onClick={() => {
          onServcieNameClick(currentWindow, "closed");
        }}
      >
        Закрыть
      </button>
    </main>
  );
};
