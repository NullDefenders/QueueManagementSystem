import { Card, Heading, HStack, Flex } from "@chakra-ui/react";

interface Props {
  time: string;
}

const TimeCard = ({ time }: Props) => {
  return (
    <Card.Root>
      <Card.Body>
        <HStack justifyContent="space-between" marginBottom={3}></HStack>
        <Flex align="center" justify="center" h="1vh" w="4.5vw">
          <Heading fontSize="2xl" letterSpacing="tight" fontWeight="bold">
            {time}
          </Heading>
        </Flex>
      </Card.Body>
    </Card.Root>
  );
};

export default TimeCard;
