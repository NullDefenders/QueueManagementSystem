import React from "react";
import type { TalonInQueue } from "../types";
import styles from "./List.module.scss";
import clsx from "clsx";

interface QueueListProps {
  queue: TalonInQueue[];
}

const QueueList: React.FC<QueueListProps> = ({ queue }) => {
  return (
    <ul className={styles.list}>
      {queue.reverse().map((talon) => (
        <li key={talon.TalonNumber} className={styles.itemContainer}>
          <span
            className={clsx(
              styles.item,
              talon.PendingTime && styles.item_withTime
            )}
          >
            {talon.TalonNumber}
          </span>
          {talon.PendingTime && (
            <span className={styles.time}>
              {talon.PendingTime.substring(0, 5)}
            </span>
          )}
        </li>
      ))}
    </ul>
  );
};

export default QueueList;
