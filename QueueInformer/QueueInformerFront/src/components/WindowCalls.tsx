import React from 'react';
import type { WindowCall } from '../types';

interface WindowCallsProps {
    calls: WindowCall[];
}

const WindowCalls: React.FC<WindowCallsProps> = ({ calls }) => {
    return (
        <ul style={{ listStyle: 'none', padding: 0, minHeight: 400}}>
            {calls.map((call, index) => (
                <li 
                    key={`${call.TalonNumber}-${call.WindowNumber}-${index}`} 
                    style={{ 
                        padding: '10px', 
                        margin: '5px 0', 
                        border: '1px solid #d4edda', 
                        borderRadius: '4px',
                        backgroundColor: '#e7f7e9'
                    }}
                >
                    <span style={{ fontWeight: 'bold', color: '#0c5460' }}>{call.TalonNumber}</span>
                    <span style={{ margin: '0 10px' }}>→</span>
                    <span style={{ fontWeight: 'bold', color: '#155724' }}>Окно {call.WindowNumber}</span>
                </li>
            ))}
        </ul>
    );
};

export default WindowCalls;