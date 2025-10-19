import { Box, Heading, SimpleGrid, Spinner, Text, VStack, Button } from "@chakra-ui/react";
import { useEffect, useState } from "react";
import useFacilityDepartments, { type Department } from "../hooks/useFacilityDepartments";

interface Props {
  facilityId: string;
  onContinue: (departmentId: string) => void;
  onLoaded?: (departments: Department[]) => void;
}

const DepartmentsGate = ({ facilityId, onContinue, onLoaded }: Props) => {
  const { data, isLoading, error } = useFacilityDepartments(facilityId);
  const [selectedDepartmentId, setSelectedDepartmentId] = useState<string>("");

  useEffect(() => {
    if (data && data.length) {
      if (onLoaded) onLoaded(data);
      // preselect first department if none selected
      setSelectedDepartmentId((prev) => prev || data[0].id);
    }
  }, [data]);

  if (isLoading) return <Spinner />;
  if (error) return <Text>Ошибка загрузки отделов: {error}</Text>;

  return (
    <Box p={4}>
      <Heading size="lg" mb={4}>Доступные отделы</Heading>
      {data.length === 0 ? (
        <Text>Для выбранного учреждения отделы не найдены.</Text>
      ) : (
        <SimpleGrid columns={{ base: 1, sm: 2, md: 3 }} gap={3}>
          {data.map((d) => {
            const isActive = d.id === selectedDepartmentId;
            return (
            <Box
              key={d.id}
              borderWidth="1px"
              borderRadius="md"
              p={3}
              onClick={() => setSelectedDepartmentId(d.id)}
              cursor="pointer"
              bg={isActive ? "black" : "white"}
              color={isActive ? "white" : "black"}
              borderColor={isActive ? "black" : "blackAlpha.300"}
            >
              <VStack align="start" gap={1}>
                <Text fontWeight={isActive ? "bold" : "semibold"}>{d.name}</Text>
                <Text fontSize="sm" color={isActive ? "whiteAlpha.800" : "fg.muted"}>ID: {d.id}</Text>
              </VStack>
            </Box>
          );})}
        </SimpleGrid>
      )}
      <Box mt={4} display="flex" justifyContent="flex-end">
        <Button variant="solid" onClick={() => onContinue(selectedDepartmentId)} disabled={!selectedDepartmentId}>Продолжить</Button>
      </Box>
    </Box>
  );
};

export default DepartmentsGate;


