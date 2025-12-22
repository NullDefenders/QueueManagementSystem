import styles from "./Ticket.module.scss";

interface TicketProps {
  serviceName: string;
  talonNumber: string;
  issuedAt: Date;
  pendingTime: string;
}

export const Ticket = (props: TicketProps) => {
  return (
    <div className={styles.ticket}>
      <span className={styles.ticket__title}>Добро пожаловать</span>
      <div className={styles.ticket__mainInfoContainer}>
        <span className={styles.ticket__service}>{props.serviceName}</span>
        <span className={styles.ticket__numberTitle}>Номер талона</span>
        <span className={styles.ticket__number}>{props.talonNumber}</span>
      </div>
      <div className={styles.ticket__dateInfoContainer}>
        <span className={styles.ticket__dateTitle}>Дата выдачи</span>
        <span className={styles.ticket__date}>
          {new Date(props.issuedAt).toLocaleString()}
        </span>
        {props.pendingTime && (
          <span className={styles.ticket__dateTitle}>Вы записаны на</span>
        )}
        {props.pendingTime && (
          <span className={styles.ticket__date}>{props.pendingTime}</span>
        )}
      </div>
    </div>
  );
};
