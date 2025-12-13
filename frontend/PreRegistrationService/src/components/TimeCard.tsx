import { Card, Heading, HStack, Flex } from "@chakra-ui/react";

interface Props {
  time: string;
  isSelected?: boolean;
  isOccupied?: boolean;
  onClick?: () => void;
}

const TimeCard = ({ time, isSelected, isOccupied, onClick }: Props) => {
  // Приоритет: занятые времена важнее выбранных
  const showOccupied = isOccupied;
  const showSelected = isSelected && !isOccupied;

  return (
    <Card.Root
      onClick={showOccupied ? undefined : onClick}
      cursor={showOccupied ? "not-allowed" : "pointer"}
      outline={showSelected ? "2px solid var(--chakra-colors-blue-500)" : showOccupied ? "2px solid var(--chakra-colors-red-500)" : "none"}
      bg={showOccupied ? "red.300" : showSelected ? "blue.50" : undefined}
      _dark={{
        bg: showOccupied ? "red.700" : showSelected ? "blue.900" : undefined,
      }}
      opacity={showOccupied ? 1 : 1}
    >
      <Card.Body>
        <HStack justifyContent="space-between" marginBottom={3}></HStack>
        <Flex align="center" justify="center" h="1vh" w="4.5vw">
          <Heading 
            fontSize="2xl" 
            letterSpacing="tight" 
            fontWeight="bold" 
            color={showOccupied ? "red.800" : showSelected ? "fg" : "fg"}
            _dark={{
              color: showOccupied ? "red.100" : showSelected ? "fg" : "fg",
            }}
          >
            {time}
          </Heading>
        </Flex>
      </Card.Body>
    </Card.Root>
  );
};

export default TimeCard;
