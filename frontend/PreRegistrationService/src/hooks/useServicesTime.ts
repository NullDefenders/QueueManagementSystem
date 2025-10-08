import { type ServiceQuery } from "../App";
import useData from "./useData";

export interface ServiceTime {
  id: number;
  name: string;
  recording_time: { time: string }[];
  engaged : number;
}

const useGames = (serviceQuery: ServiceQuery) =>
  useData<ServiceTime>(
    "/serviceTimes",
    {
      params: {
        services: serviceQuery.service?.id,
        ordering: serviceQuery.sortOrder,
        search: serviceQuery.searchText
      },
    },
    [serviceQuery]
  );

export default useGames;
