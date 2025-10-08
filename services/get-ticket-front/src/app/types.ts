export const enum Steps {
  Departments,
  Categories,
  Services,
  Ticket,
}

export interface IDepartment {
  id: string;
  name: string;
  address: string;
}

export interface IService {
  categoryCode: string;
  categoryId: string;
  categoryName: string;
  serviceCode: string;
  serviceId: string;
  serviceName: string;
}

export interface ITicket {
  id: number;
  issuedAt: Date;
  serviceName: string;
  ticketNumber: string;
}
