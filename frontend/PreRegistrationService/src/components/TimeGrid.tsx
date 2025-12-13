import { SimpleGrid, Text, Box } from "@chakra-ui/react";
import { type ServiceQuery } from "../App";
import TimeCard from "./TimeCard";
import TimeCardContainer from "./TimeCardContainer";
import useOccupiedTimes from "../hooks/useOccupiedTimes";
import { useEffect } from "react";

interface Props {
  serviceQuery: ServiceQuery;
  selectedTime?: string;
  onSelectTime?: (time: string) => void;
}

const TimeGrid = ({ serviceQuery, selectedTime, onSelectTime }: Props) => {
  const { occupiedTimes, error, isLoading } = useOccupiedTimes(
    serviceQuery.date,
    serviceQuery.service?.serviceId
  );

  // Логирование для отладки
  useEffect(() => {
    console.log("TimeGrid - занятые времена:", occupiedTimes);
    console.log("TimeGrid - количество занятых времен:", occupiedTimes.length);
    console.log("TimeGrid - дата:", serviceQuery.date);
    console.log("TimeGrid - serviceId:", serviceQuery.service?.serviceId);
    if (error) {
      console.error("TimeGrid - ошибка:", error);
    }
  }, [occupiedTimes, serviceQuery.date, serviceQuery.service?.serviceId, error]);

  const timeArray = Array.from(
    Array.from({ length: 36 }, (_, i) => 32 + i),
    (x) => x * 15
  );

  const formatTime = (time: number) => {
    const hours = Math.floor(time / 60);
    const minutes = time % 60;
    return `${hours.toString().padStart(2, "0")}:${minutes.toString().padStart(2, "0")}`;
  };

  const isTimeOccupied = (time: string) => {
    const result = occupiedTimes.includes(time);
    if (result) {
      console.log(`Время ${time} занято (найдено в массиве)`);
    }
    return result;
  };

  return (
    <>
      {error && (
        <Box color="red.500" padding="10px" bg="red.50" _dark={{ bg: "red.900", color: "red.200" }} borderRadius="md" mb={2}>
          Ошибка: {error}
        </Box>
      )}
      {isLoading && (
        <Box padding="10px" color="fg.muted">
          Загрузка занятых времен...
        </Box>
      )}
      <SimpleGrid
        columns={{ sm: 1, md: 2, lg: 3, xl: 4 }}
        padding="24px"
        gap="10px"
      >
        {timeArray.map((time) => {
          const timeString = formatTime(time);
          const isOccupied = isTimeOccupied(timeString);
          
          return (
            <TimeCardContainer key={timeString}>
              <TimeCard
                time={timeString}
                isSelected={selectedTime === timeString}
                isOccupied={isOccupied}
                onClick={() => !isOccupied && onSelectTime && onSelectTime(timeString)}
              />
            </TimeCardContainer>
          );
        })}
      </SimpleGrid>
    </>
  );
};

export default TimeGrid;
