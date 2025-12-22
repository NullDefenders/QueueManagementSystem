import { type AxiosRequestConfig, CanceledError } from "axios";
import { useEffect, useState } from "react";
import apiClient from "../services/api-client";

interface FetchResponse<T> {
  count: number;
  services: T[];
}

const useData = <T>(endpoint: string, requestConfig?: AxiosRequestConfig, deps?: any[]) => {
  const [data, setData] = useState<T[]>([]);
  const [error, setError] = useState("");
  const [isLoading, setLoading] = useState(false);

  useEffect(() => {
    const controller = new AbortController();

    console.log('useData: Making request to', endpoint, 'with config:', requestConfig);
    setLoading(true);
    apiClient
      .get<FetchResponse<T>>(endpoint, { signal: controller.signal, ...requestConfig })
      .then((res) => {
        console.log('useData: Response received', res.data);
        setData(res.data.services);
        setLoading(false);
      })
      .catch((err) => {
        if (err instanceof CanceledError) return;
        console.error('useData: Request failed', err);
        setError(err.message)
        setLoading(false);
      });

    return () => controller.abort();
  }, deps ? deps : []);

  return { data, error, isLoading };
};

export default useData;