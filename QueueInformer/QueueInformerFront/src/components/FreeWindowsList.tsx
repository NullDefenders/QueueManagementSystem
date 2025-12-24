import React from "react";

import styles from "./List.module.scss";

interface FreeWindowsListProps {
  windows: string[];
}

const FreeWindowsList: React.FC<FreeWindowsListProps> = ({ windows }) => {
  return (
    <div style={{ padding: 0 }}>
      {windows.length > 0 ? (
        <div className={styles.list}>
          {windows.map((num) => (
            <span key={num} className={styles.item}>
              Окно {num}
            </span>
          ))}
        </div>
      ) : (
        <p style={{ color: "#555", margin: "5px 0" }}>Нет свободных окон.</p>
      )}
    </div>
  );
};

export default FreeWindowsList;
