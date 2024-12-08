import React from 'react';
import logo from './logo.svg';
import './App.css';
import InputForm from './components/InputForm';

function App() {
  const valueArray = [
    {title: 'TITLE-A', content: 'CONTENT-A'},
    {title: 'TITLE-B', content: 'CONTENT-B'},
    {title: 'TITLE-C', content: 'CONTENT-C'}
  ];
  const str1 : string | string[] = 'Test String';


  return (
    <div className="App">
      <InputForm/>      
    </div>
  );
}

export default App;
