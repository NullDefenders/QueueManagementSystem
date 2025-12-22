import { Card, SkeletonText } from "@chakra-ui/react";

const TimeCardSkeleton = () => {
  return (
    <Card.Root>
      <Card.Body>
        <SkeletonText />
      </Card.Body>
    </Card.Root>
  );
};

export default TimeCardSkeleton;
