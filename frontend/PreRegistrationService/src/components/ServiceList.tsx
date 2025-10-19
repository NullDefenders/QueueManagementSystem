import { Box, Button, Heading, SimpleGrid, Spinner, Text, VStack } from "@chakra-ui/react";
import useServices, { type Service } from "../hooks/useServices";
import { type ServiceQuery } from "../App";

interface Props {
  onSelectService: (service: Service) => void;
  selectedService: Service | null;
  serviceQuery: ServiceQuery;
}

const ServiceList = ({
  selectedService,
  onSelectService,
  serviceQuery,
}: Props) => {
  const { data, isLoading, error } = useServices(serviceQuery);

  console.log('ServiceList render:', { data, isLoading, error, serviceQuery });

  if (error) {
    console.error('ServiceList error:', error);
    return <div>Ошибка загрузки: {error}</div>;
  }

  if (isLoading) return <Spinner />;

  return (
    <>
      <Heading fontSize="2xl" marginTop={9} marginBottom={3}>
        Услуги
      </Heading>
      <Box>
        <SimpleGrid columns={{ base: 1, lg: 3, xl: 4 }} gap={3}>
          {data?.map((service) => {
            const isActive = service.serviceId === selectedService?.serviceId;
            return (
              <Box key={service.serviceId}>
                <Button
                  onClick={() => onSelectService(service)}
                  justifyContent="flex-start"
                  variant="ghost"
                  w="full"
                  px={4}
                  py={4}
                  borderRadius="md"
                  bg={isActive ? "black" : "white"}
                  color={isActive ? "white" : "black"}
                  borderWidth="1px"
                  borderColor={isActive ? "black" : "blackAlpha.300"}
                  _hover={{ bg: isActive ? "blackAlpha.800" : "blackAlpha.50", borderColor: isActive ? "black" : "blackAlpha.400" }}
                  _focusVisible={{ boxShadow: "0 0 0 2px var(--chakra-colors-blackAlpha-400)" }}
                  h="166px"
                  whiteSpace="normal"
                  alignItems="stretch"
                >
                  <VStack align="start" gap={1} w="full">
                    <Text
                      fontWeight={isActive ? "bold" : "semibold"}
                      lineClamp={3}
                      wordBreak="break-word"
                      lineHeight="1.25"
                    >
                      {service.serviceName}
                    </Text>
                    {service.categoryName && (
                      <Text
                        fontSize="sm"
                        opacity={isActive ? 0.9 : 0.8}
                        lineClamp={2}
                        color={isActive ? "white" : "blackAlpha.800"}
                        wordBreak="break-word"
                        lineHeight="1.2"
                      >
                        {service.categoryName}
                      </Text>
                    )}
                  </VStack>
                </Button>
              </Box>
            );
          })}
        </SimpleGrid>
      </Box>
    </>
  );
};

export default ServiceList;
