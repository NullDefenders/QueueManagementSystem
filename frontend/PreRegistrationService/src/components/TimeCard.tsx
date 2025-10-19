import { Card, Heading, HStack, Flex } from "@chakra-ui/react";

interface Props {
  time: string;
  isSelected?: boolean;
  onClick?: () => void;
}

const TimeCard = ({ time, isSelected, onClick }: Props) => {
  return (
    <Card.Root
      onClick={onClick}
      cursor="pointer"
      outline={isSelected ? "2px solid var(--chakra-colors-blue-500)" : "none"}
      bg={isSelected ? "blue.50" : undefined}
    >
      <Card.Body>
        <HStack justifyContent="space-between" marginBottom={3}></HStack>
        <Flex align="center" justify="center" h="1vh" w="4.5vw">
          <Heading fontSize="2xl" letterSpacing="tight" fontWeight="bold" color={isSelected ? "black" : undefined}>
            {time}
          </Heading>
        </Flex>
      </Card.Body>
    </Card.Root>
  );
};

export default TimeCard;
