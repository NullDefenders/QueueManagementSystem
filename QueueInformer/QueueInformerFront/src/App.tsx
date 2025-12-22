import EventMonitor from './components/EventMonitor';

function App() {
  return (
    <div className="App" style={{ fontFamily: 'Arial, sans-serif', height: '100vh', width: '100vw', display: 'flex', flexDirection: 'column', textAlign: 'center'}}>
      <h1>Монитор Очереди</h1>
      <hr style={{ margin: '20px 0' }} />
      <EventMonitor />
    </div>
  );
}
export default App;