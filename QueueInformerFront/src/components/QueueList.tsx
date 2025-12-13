import React from 'react';
import type { TalonInQueue } from '../types';

interface QueueListProps {
    queue: TalonInQueue[];
}

const QueueList: React.FC<QueueListProps> = ({ queue }) => {
    return (
        <ul style={{ listStyle: 'none', padding: 0, height: '45%' }}>
            {queue.map(talon => (
                <li 
                    key={talon.TalonNumber}
                    style={{ 
                        padding: '10px', 
                        margin: '5px 0', 
                        border: '1px solid #ddd', 
                        borderRadius: '4px',
                        backgroundColor: '#f9f9f9'
                    }}
                >
                    <strong>Номер талона: {talon.TalonNumber}</strong>
                    {talon.PendingTime && (
                        <span style={{ marginLeft: '10px', color: '#555' }}>
                            (Время записи: {talon.PendingTime})
                        </span>
                    )}
                </li>
            ))}
        </ul>
    );
};

export default QueueList;