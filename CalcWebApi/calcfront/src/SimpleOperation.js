import React, { useState } from 'react';

const SimpleOperation = ({ endpoint, operation }) => {
    const [num1, setNum1] = useState('');
    const [num2, setNum2] = useState('');
    const [result, setResult] = useState('');
    const [error, setError] = useState('');

    const handleCalculate = async () => {
        try {
            const response = await fetch(`https://localhost:7175/calc/${endpoint}?firstValue=${num1}&secondValue=${num2}`);
            const data = await response.json();

            if (response.ok) {
                setResult(` = ${data}`);
                setError('');
            } else {
                let errorMessage = '';

                if (data.errors) {
                    if (data.errors.firstValue) {
                        errorMessage = data.errors.firstValue[0];
                    } else if (data.errors.secondValue) {
                        errorMessage = data.errors.secondValue[0];
                    }
                }

                setResult('');
                setError(errorMessage);
            }
        } catch (error) {
            console.error(error);
        }
    };

    return (
        <div style={{
            display: 'flex',
            marginBottom: '5vw',
            marginTop: '5vw',
            marginLeft: '10vw'
        }}>
            <input
                type="text"
                value={num1}
                onChange={(e) => setNum1(e.target.value)}
                style={{
                    width: '10vw', height: '5vw', textAlign: 'center', fontSize: '2vw' }}
            />
            <button style={{
                width: '5vw',
                height: '5vw',
                padding: '1vw',
                border: 'none',
                marginRight: '2vw',
                marginLeft: '2vw',
                fontSize: '2vw',
            }} onClick={handleCalculate}>{operation}</button>
            <input
                type="text"
                value={num2}
                onChange={(e) => setNum2(e.target.value)}
                style={{
                    width: '10vw',
                    height: '5vw',
                    textAlign: 'center',
                    fontSize: '2vw'
                }}
            />
            <p style={{
                marginLeft: '2vw',
                fontSize: '1.5vw',
                marginTop: '1vw'
            }}>{result}</p>
            <p style={{
                color: 'red',
                fontSize: '1.5vw',
                marginTop: '1vw'
            }}>{error}</p>
        </div>
    );
};

export default SimpleOperation;