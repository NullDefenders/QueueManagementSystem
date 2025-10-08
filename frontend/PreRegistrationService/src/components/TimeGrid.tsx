import { SimpleGrid, Text } from "@chakra-ui/react";
import { type ServiceQuery } from "../App";
import useServicesTime from "../hooks/useServicesTime";
import TimeCard from "./TimeCard";
import TimeCardContainer from "./TimeCardContainer";
import TimeCardSkeleton from "./TimeCardSkeleton";

interface Props {
  serviceQuery: ServiceQuery;
}

const TimeGrid = ({ serviceQuery }: Props) => {
  //const { data, error, isLoading } = useServicesTime(serviceQuery);
  const skeletons = [1, 2, 3, 4, 5, 6];
  const timeArray = Array.from(
    Array.from({ length: 36 }, (_, i) => 32 + i),
    (x) => x * 15
  );

  //if (error) return <Text>{error}</Text>;

  return (
    <SimpleGrid
      columns={{ sm: 1, md: 2, lg: 3, xl: 4 }}
      padding="200px"
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
          />
        </TimeCardContainer>
      ))}
    </SimpleGrid>
  );
};

export default TimeGrid;
