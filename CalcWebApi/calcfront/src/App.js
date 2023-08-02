import logo from './logo.svg';
import './App.css';
import SimpleOperation from './SimpleOperation';
import MathExpression from './MathExpression';

function App() {
  return (
      <div className="App" >
          <SimpleOperation endpoint="addition" operation="+" />
          <SimpleOperation endpoint="subtraction" operation="-" />
          <SimpleOperation endpoint="multiplication" operation="*" />
          <SimpleOperation endpoint="division" operation="/" />
          <SimpleOperation endpoint="root" operation="&radic;" />
          <SimpleOperation endpoint="exponentiation" operation="^" />
          <MathExpression endpoint="evaluate" operation="=" />
    </div>
  );
}

export default App;
