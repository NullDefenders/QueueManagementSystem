import { Card, Heading, HStack, Flex } from "@chakra-ui/react";

interface Props {
  time: string;
  isSelected?: boolean;
  isOccupied?: boolean;
  onClick?: () => void;
}

const TimeCard = ({ time, isSelected, isOccupied, onClick }: Props) => {
  return (
    <Card.Root
      onClick={isOccupied ? undefined : onClick}
      cursor={isOccupied ? "not-allowed" : "pointer"}
      outline={isSelected ? "2px solid var(--chakra-colors-blue-500)" : "none"}
      bg={isOccupied ? "red.100" : isSelected ? "blue.50" : undefined}
      opacity={isOccupied ? 0.6 : 1}
    >
      <Card.Body>
        <HStack justifyContent="space-between" marginBottom={3}></HStack>
        <Flex align="center" justify="center" h="1vh" w="4.5vw">
          <Heading 
            fontSize="2xl" 
            letterSpacing="tight" 
            fontWeight="bold" 
            color={isOccupied ? "red.600" : isSelected ? "black" : undefined}
          >
            {time}
          </Heading>
        </Flex>
      </Card.Body>
    </Card.Root>
  );
};

export default TimeCard;
