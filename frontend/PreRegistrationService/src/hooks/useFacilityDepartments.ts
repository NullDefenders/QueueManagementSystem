import { useEffect, useState } from "react";
import axios from "axios";

export interface Department {
  id: string;
  name: string;
}

interface DepartmentsResponse {
  departments?: Department[];
  items?: Department[];
  data?: Department[];
}

const useFacilityDepartments = (facilityId?: string) => {
  const [data, setData] = useState<Department[]>([]);
  const [error, setError] = useState("");
  const [isLoading, setLoading] = useState(false);

  useEffect(() => {
    if (!facilityId) return;
    
    const controller = new AbortController();
    setLoading(true);
    
    axios
      .get<DepartmentsResponse | Department[]>(`http://localhost:5259/api/facility/departments/${facilityId}`, { 
        signal: controller.signal 
      })
      .then((res) => {
        const responseData = res.data;
        let departmentsArray: Department[] = [];

        // Обрабатываем разные форматы ответа
        if (Array.isArray(responseData)) {
          departmentsArray = responseData;
        } else if (responseData && Array.isArray(responseData.departments)) {
          departmentsArray = responseData.departments;
        } else if (responseData && Array.isArray(responseData.items)) {
          departmentsArray = responseData.items;
        } else if (responseData && Array.isArray(responseData.data)) {
          departmentsArray = responseData.data;
        } else {
          console.warn('Unexpected API response format:', responseData);
        }

        setData(departmentsArray);
        setLoading(false);
      })
      .catch((err) => {
        if (err.name === "CanceledError") return;
        setError(err.message ?? "Failed to load departments");
        setLoading(false);
      });
      
    return () => controller.abort();
  }, [facilityId]);

  return { data, error, isLoading };
};

export default useFacilityDepartments;


