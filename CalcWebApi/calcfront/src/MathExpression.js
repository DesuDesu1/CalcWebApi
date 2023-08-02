import React, { useState } from 'react';

const MathExpression = ({ endpoint, operation }) => {
    const [expression, setExpression] = useState('');
    const [result, setResult] = useState('');
    const [error, setError] = useState('');

    const handleEvaluation = async () => {
        try {
            const encodedExpression = encodeURIComponent(expression);
            const response = await fetch(`https://localhost:7175/calc/${endpoint}?expression=${encodedExpression}`);
            const data = await response.json();

            if (response.ok) {
                setResult(` ${data}`);
                console.log('succes');
                setError('');
            }
            else {
                console.log('2 HERE HERE')

                let errorMessage = '';
                if (data.errors) {
                    errorMessage = data.errors.expression[0];
                }
                if (data.value.firstValue) {
                    errorMessage = data.value.firstValue[0];
                }
                if (data.value.secondValue) {
                    errorMessage = data.value.secondValue[0];
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
                value={expression}
                onChange={(e) => setExpression(e.target.value)}
                style={{
                    width: '20vw',
                    height: '5vw',
                    textAlign: 'center',
                    fontSize: '2vw'
                }}
            />
            <button
                style={{
                    width: '5vw',
                    height: '5vw',
                    padding: '1vw',
                    border: 'none',
                    marginRight: '1vw',
                    marginLeft: '2vw',
                    fontSize: '2vw',
                }}
                onClick={handleEvaluation}
            >
                {operation}
            </button>
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

export default MathExpression;