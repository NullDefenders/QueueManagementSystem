import { Button, Heading, HStack, List, Spinner } from "@chakra-ui/react";
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
      <List.Root>
        {data?.map((service) => (
          <List.Item key={service.serviceId} paddingY="5px">
            <HStack>
              <Button
                whiteSpace="normal"
                textAlign="left"
                fontWeight={
                  service.serviceId === selectedService?.serviceId
                    ? "bold"
                    : "normal"
                }
                onClick={() => onSelectService(service)}
                fontSize="md"
                variant="solid"
              >
                {service.serviceName}
              </Button>
            </HStack>
          </List.Item>
        ))}
      </List.Root>
    </>
  );
};

export default ServiceList;
