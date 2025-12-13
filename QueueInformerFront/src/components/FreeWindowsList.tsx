import React from 'react';

interface FreeWindowsListProps {
    windows: string[];
}

const FreeWindowsList: React.FC<FreeWindowsListProps> = ({ windows }) => {
    return (
        <div style={{ padding: 0}}>
            {windows.length > 0 ? (
                <div style={{ display: 'flex', flexWrap: 'wrap', gap: '10px', marginTop: '10px' }}>
                    {windows.map(num => (
                        <span 
                            key={num} 
                            style={{ 
                                padding: '5px 10px', 
                                backgroundColor: '#155724', 
                                color: 'white', 
                                borderRadius: '3px',
                                fontWeight: 'bold'
                            }}
                        >
                            Окно {num}
                        </span>
                    ))}
                </div>
            ) : (
                <p style={{ color: '#555', margin: '5px 0' }}>Нет свободных окон.</p>
            )}
        </div>
    );
};

export default FreeWindowsList;