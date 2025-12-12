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
    return <Text color="red.500">Ошибка загрузки: {error}</Text>;
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
                  bg={isActive ? "colorPalette.600" : "bg"}
                  color={isActive ? "white" : "fg"}
                  borderWidth="1px"
                  borderColor={isActive ? "colorPalette.600" : "border"}
                  _hover={{ 
                    bg: isActive ? "colorPalette.700" : "bg.subtle", 
                    borderColor: isActive ? "colorPalette.700" : "border.emphasized" 
                  }}
                  _dark={{
                    bg: isActive ? "colorPalette.500" : "bg",
                    color: isActive ? "white" : "fg",
                    _hover: {
                      bg: isActive ? "colorPalette.600" : "bg.subtle",
                    }
                  }}
                  _focusVisible={{ boxShadow: "0 0 0 2px var(--chakra-colors-colorPalette-400)" }}
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
                      color={isActive ? "white" : "fg"}
                    >
                      {service.serviceName}
                    </Text>
                    {service.categoryName && (
                      <Text
                        fontSize="sm"
                        opacity={isActive ? 0.9 : 0.8}
                        lineClamp={2}
                        color={isActive ? "whiteAlpha.800" : "fg.muted"}
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
