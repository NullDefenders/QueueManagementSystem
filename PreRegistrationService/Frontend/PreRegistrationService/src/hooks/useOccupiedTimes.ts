import { useEffect, useState } from "react";
import axios from "axios";

export interface OccupiedTime {
  time: string; // Формат HH:mm
}

const useOccupiedTimes = (date: string | undefined, serviceId: string | undefined) => {
  const [occupiedTimes, setOccupiedTimes] = useState<string[]>([]);
  const [error, setError] = useState("");
  const [isLoading, setLoading] = useState(false);

  useEffect(() => {
    if (!date || !serviceId) {
      setOccupiedTimes([]);
      return;
    }

    const controller = new AbortController();
    setLoading(true);
    setError("");

    // Форматируем дату в YYYY-MM-DD (заменяем подчеркивание на дефис, если есть)
    const formattedDate = date.replace(/_/g, "-");

    const url = `https://localhost:44345/allrecordtimeinday/${formattedDate}/${serviceId}`;
    console.log("Запрос занятых времен:", url);

    axios
      .get(url, {
        signal: controller.signal,
      })
      .then((res) => {
        console.log("Ответ API занятых времен:", res.data);
        
        let times: string[] = [];
        
        // Обрабатываем разные форматы ответа
        if (Array.isArray(res.data)) {
          if (res.data.length === 0) {
            times = [];
          } else if (typeof res.data[0] === "string") {
            // Если массив строк
            times = res.data;
          } else if (typeof res.data[0] === "object" && res.data[0].time) {
            // Если массив объектов с полем time
            times = res.data.map((item: any) => item.time);
          } else if (typeof res.data[0] === "object" && res.data[0].recordTime) {
            // Если массив объектов с полем recordTime
            times = res.data.map((item: any) => {
              const timeStr = item.recordTime;
              // Извлекаем время из ISO строки или используем как есть
              if (timeStr.includes("T")) {
                return timeStr.split("T")[1].substring(0, 5); // HH:mm
              }
              return timeStr;
            });
          }
        }
        
        // Нормализуем формат времени к HH:mm
        times = times.map((time) => {
          // Если время в формате HH:mm:ss, обрезаем до HH:mm
          if (time.length > 5) {
            return time.substring(0, 5);
          }
          return time;
        });
        
        console.log("Обработанные занятые времена:", times);
        setOccupiedTimes(times);
        setLoading(false);
      })
      .catch((err) => {
        if (err.name !== "CanceledError") {
          console.error("Ошибка загрузки занятых времен:", err);
          console.error("Детали ошибки:", err.response?.data || err.message);
          setError(err.message);
          setOccupiedTimes([]);
        }
        setLoading(false);
      });

    return () => controller.abort();
  }, [date, serviceId]);

  return { occupiedTimes, error, isLoading };
};

export default useOccupiedTimes;
