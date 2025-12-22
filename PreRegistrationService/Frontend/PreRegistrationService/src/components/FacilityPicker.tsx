import { Box, Button, Heading, SimpleGrid, Spinner, Text, VStack } from "@chakra-ui/react";
import useFacilities, { type Facility } from "../hooks/useFacilities";

interface Props {
  onSelect: (facility: Facility) => void;
}

const FacilityPicker = ({ onSelect }: Props) => {
  const { data, isLoading, error } = useFacilities();

  if (isLoading) return <Spinner />;
  if (error) return <Text>Ошибка загрузки: {error}</Text>;

  return (
    <Box p={4}>
      <Heading size="lg" mb={4}>Выберите учреждение</Heading>
      <SimpleGrid columns={{ base: 1, sm: 2, md: 3 }} gap={3}>
        {data.map((f) => (
          <Box key={f.id}>
            <Button
              onClick={() => onSelect(f)}
              justifyContent="flex-start"
              variant="subtle"
              w="full"
              px={14}
              py={14}
              borderRadius="md"
              borderWidth="1px"
            >
              <VStack align="start" gap={1} w="full">
                <Text fontWeight="semibold">{f.name}</Text>
                {f.id && (
                  <Text fontSize="sm" color="fg.muted">ID: {f.id}</Text>
                )}
              </VStack>
            </Button>
          </Box>
        ))}
      </SimpleGrid>
    </Box>
  );
};

export default FacilityPicker;


