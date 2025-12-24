import EventMonitor from "./components/EventMonitor";

import "./App.css";
import logo from "./logo-queue-1.png";

function App() {
  return (
    <div className="app">
      <div className="app__header">
        <img src={logo} width={400} height={208} />
      </div>
      <EventMonitor />
    </div>
  );
}
export default App;
