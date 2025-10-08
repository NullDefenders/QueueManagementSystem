//import services from "../data/services.ts";
import useData from "./useData.ts";
import { type ServiceQuery } from "../App.tsx";

export interface Service {
  categoryId: string;
  categoryName: string;
  serviceId: string;
  serviceCode: string;
  serviceName: string;
}

//const useServices = () => ({ data: services, isLoading: false, error: null })
const useServices = (serviceQuery: ServiceQuery) => {
  // Создаем объект параметров только с определенными значениями
  const params: Record<string, string> = {};
  if (serviceQuery.service?.categoryId) params.categoryId = serviceQuery.service.categoryId;
  if (serviceQuery.service?.categoryName) params.categoryName = serviceQuery.service.categoryName;
  if (serviceQuery.service?.serviceId) params.serviceId = serviceQuery.service.serviceId;
  if (serviceQuery.service?.serviceName) params.serviceName = serviceQuery.service.serviceName;

  return useData<Service>("/department/services/676885cb-ee0f-40b4-be03-9d25a8a2dca1", {
    params: Object.keys(params).length > 0 ? params : undefined
  });
};

export default useServices;