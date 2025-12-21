import React, { useState, useEffect } from "react";
import QueueList from "./QueueList";
import WindowCalls from "./WindowCalls";
import FreeWindowsList from "./FreeWindowsList";
import type {
  TalonInQueue,
  WindowCall,
  NewTalonMessage,
  CallMessage,
  WindowStatusMessage,
  EventMessage,
} from "../types";

const EventMonitor: React.FC = () => {
  const [queueMap, setQueueMap] = useState<Map<string, TalonInQueue>>(
    () => new Map()
  );

  const [callsMap, setCallsMap] = useState<Map<string, WindowCall>>(
    () => new Map()
  );

  const [freeWindows, setFreeWindows] = useState<Set<string>>(() => new Set());

  useEffect(() => {
    const eventSource = new EventSource("http://localhost:8086/api/events");

    eventSource.onopen = () => {
      console.log("Соединение с SSE установлено.");
    };

    eventSource.onerror = (error) => {
      console.error("Ошибка SSE:", error);
    };

    eventSource.onmessage = (event) => {
      try {
        const data: EventMessage = JSON.parse(event.data);

        if ("ServiceCode" in data) {
          handleNewTalon(data as NewTalonMessage);
        } else if ("WindowNumber" in data && "TalonNumber" in data) {
          handleTalonCall(data as CallMessage);
        } else if ("Status" in data && "WindowNumber" in data) {
          handleWindowStatusUpdate(data as WindowStatusMessage);
        }
      } catch (e) {
        console.error("Ошибка парсинга JSON:", e);
      }
    };

    return () => {
      eventSource.close();
    };
  }, []);

  const handleNewTalon = (newTalon: NewTalonMessage) => {
    const talonKey = newTalon.TalonNumber;
    const talonValue: TalonInQueue = {
      TalonNumber: talonKey,
      PendingTime: newTalon.PendingTime,
    };

    setQueueMap((prevMap) => {
      const newMap = new Map(prevMap);
      newMap.set(talonKey, talonValue);
      return newMap;
    });
  };

  const handleTalonCall = (callData: CallMessage) => {
    const talonKey = callData.TalonNumber;
    const windowKey = callData.WindowNumber;
    const callValue: WindowCall = {
      TalonNumber: talonKey,
      WindowNumber: windowKey,
    };

    setCallsMap((prevMap) => {
      const newMap = new Map(prevMap);
      newMap.set(windowKey, callValue);
      return newMap;
    });

    setFreeWindows((prevSet) => {
      const newSet = new Set(prevSet);
      newSet.delete(windowKey);
      return newSet;
    });

    setQueueMap((prevMap) => {
      const newMap = new Map(prevMap);
      newMap.delete(talonKey);
      return newMap;
    });
  };
  const handleWindowStatusUpdate = (statusData: WindowStatusMessage) => {
    const windowKey = statusData.WindowNumber;

    if (statusData.Status === "busy" || statusData.Status === "free") {
      setCallsMap((prevMap) => {
        const newMap = new Map(prevMap);
        newMap.delete(windowKey);
        return newMap;
      });
    }

    if (statusData.Status === "free") {
      setFreeWindows((prevSet) => {
        const newSet = new Set(prevSet);
        newSet.add(windowKey);
        return newSet;
      });
    } else if (statusData.Status === "busy") {
      setFreeWindows((prevSet) => {
        const newSet = new Set(prevSet);
        newSet.delete(windowKey);
        return newSet;
      });
    }
  };

  const queueArray = Array.from(queueMap.values()).reverse();
  const callsArray = Array.from(callsMap.values());
  const freeWindowsArray = Array.from(freeWindows.values());

  return (
    <div
      style={{
        display: "flex",
        justifyContent: "space-around",
        padding: "20px",
      }}
    >
      <div
        style={{
          width: "45%",
          borderRight: "1px solid #ccc",
          paddingRight: "20px",
        }}
      >
        <h2 style={{ marginTop: "20px" }}>Очередь</h2>
        <QueueList queue={queueArray} />
        <h2 style={{ marginTop: "20px" }}>Свободные окна</h2>
        <FreeWindowsList windows={freeWindowsArray} />
      </div>

      <div style={{ width: "45%", paddingLeft: "20px" }}>
        <h2>Текущие вызовы</h2>
        <WindowCalls calls={callsArray} />
      </div>
    </div>
  );
};

export default EventMonitor;
