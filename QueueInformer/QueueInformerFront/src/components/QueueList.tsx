import React from "react";
import type { TalonInQueue } from "../types";
import styles from "./List.module.scss";

interface QueueListProps {
  queue: TalonInQueue[];
}

const QueueList: React.FC<QueueListProps> = ({ queue }) => {
  return (
    <ul className={styles.list}>
      {queue.map((talon) => (
        <li key={talon.TalonNumber}>
          <span className={styles.item}>{talon.TalonNumber}</span>
          {talon.PendingTime && (
            <span
              style={{
                marginLeft: "10px",
                color: "var(--secondary-text-color, #555)",
              }}
            >
              | {talon.PendingTime}
            </span>
          )}
        </li>
      ))}
    </ul>
  );
};

export default QueueList;
