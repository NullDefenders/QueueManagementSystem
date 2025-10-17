import styles from "./Ticket.module.scss";

interface TicketProps {
  serviceName: string;
  talonNumber: string;
  issuedAt: Date;
}

export const Ticket = (props: TicketProps) => {
  return (
    <div className={styles.ticket}>
      <span className={styles.ticket__title}>Добро пожаловать</span>
      <span className={styles.ticket__service}>{props.serviceName}</span>
      <span className={styles.ticket__numberTitle}>Талон номер</span>
      <span className={styles.ticket__number}>{props.talonNumber}</span>
      <span className={styles.ticket__dateTitle}>Дата выдачи</span>
      <span className={styles.ticket__date}>
        {new Date(props.issuedAt).toLocaleString()}
      </span>
    </div>
  );
};
