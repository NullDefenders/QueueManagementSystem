import { useEffect, useState } from "react";
import axios from "axios";

export interface Facility {
  id: string;
  name: string;
  code?: string;
}

const FACILITIES_URL = "http://localhost:5259/api/facilities";

const useFacilities = () => {
  const [data, setData] = useState<Facility[]>([]);
  const [error, setError] = useState("");
  const [isLoading, setLoading] = useState(false);

  useEffect(() => {
    const controller = new AbortController();
    setLoading(true);
    axios
      .get(FACILITIES_URL, { signal: controller.signal }) // Без указания типа
      .then((res) => {
        const responseData = res.data;
        
        // Пытаемся найти массив в разных возможных полях
        let facilitiesArray: Facility[] = [];
        
        if (Array.isArray(responseData)) {
          facilitiesArray = responseData;
        } else if (responseData && Array.isArray(responseData.facilities)) {
          facilitiesArray = responseData.facilities;
        } else if (responseData && Array.isArray(responseData.items)) {
          facilitiesArray = responseData.items;
        } else if (responseData && Array.isArray(responseData.data)) {
          facilitiesArray = responseData.data;
        }
        
        if (facilitiesArray.length > 0) {
          setData(facilitiesArray);
        } else {
          console.error('No facilities array found in response:', responseData);
          setData([]);
          setError('Неверный формат данных');
        }
        setLoading(false);
      })
      .catch((err) => {
        if (err.name === "CanceledError") return;
        setError(err.message ?? "Failed to load facilities");
        setLoading(false);
      });
    return () => controller.abort();
  }, []);

  return { data, error, isLoading };
};

export default useFacilities;


