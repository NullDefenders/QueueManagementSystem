export interface TalonInQueue {
    TalonNumber: string;
    PendingTime?: string;
}

export interface WindowCall {
    TalonNumber: string;
    WindowNumber: string;
}

export interface NewTalonMessage {
    TalonNumber: string;
    ServiceCode: string; 
    PendingTime?: string;
}

export interface CallMessage {
    TalonNumber: string;
    WindowNumber: string;
}

export interface WindowStatusMessage {
    WindowNumber: string;
    Status: string;
}

export type EventMessage = NewTalonMessage | CallMessage | WindowStatusMessage;