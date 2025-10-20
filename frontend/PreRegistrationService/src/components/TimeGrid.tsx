import { SimpleGrid } from "@chakra-ui/react";
import { type ServiceQuery } from "../App";
import TimeCard from "./TimeCard";
import TimeCardContainer from "./TimeCardContainer";

interface Props {
  serviceQuery: ServiceQuery;
  selectedTime?: string;
  onSelectTime?: (time: string) => void;
}

const TimeGrid = ({ selectedTime, onSelectTime }: Props) => {
  //const { data, error, isLoading } = useServicesTime(serviceQuery);
  const timeArray = Array.from(
    Array.from({ length: 36 }, (_, i) => 32 + i),
    (x) => x * 15
  );

  //if (error) return <Text>{error}</Text>;

  return (
    <SimpleGrid
      columns={{ sm: 1, md: 2, lg: 3, xl: 4 }}
      padding="24px"
      gap="10px"
    >
      {/*isLoading &&
        skeletons.map((skeleton) => (
          <TimeCardContainer key={skeleton}>
            <TimeCardSkeleton />
          </TimeCardContainer>
        ))*/}
      {timeArray.map((time) => (
        <TimeCardContainer key={`${Math.floor(time / 60)}:${time % 60}`}>
          <TimeCard
            time={`${Math.floor(time / 60)}:${time % 60}${
              time % 60 === 0 ? "0" : ""
            }`}
            isSelected={
              selectedTime ===
              `${Math.floor(time / 60)}:${time % 60}${
                time % 60 === 0 ? "0" : ""
              }`
            }
            onClick={() =>
              onSelectTime &&
              onSelectTime(
                `${Math.floor(time / 60)}:${time % 60}${
                  time % 60 === 0 ? "0" : ""
                }`
              )
            }
          />
        </TimeCardContainer>
      ))}
    </SimpleGrid>
  );
};

export default TimeGrid;
