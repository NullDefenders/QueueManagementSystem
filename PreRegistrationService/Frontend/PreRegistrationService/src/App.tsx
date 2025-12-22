import { Box, Grid, GridItem, HStack, Heading, Text, IconButton, Slider, VStack, Button, Dialog, Input } from "@chakra-ui/react";
import { useMemo, useState } from "react";
import apiClient from "./services/api-client";
import TimeGrid from "./components/TimeGrid";
import "./App.css";
import ServiceList from "./components/ServiceList";
import FacilityPicker from "./components/FacilityPicker";
import DepartmentsGate from "./components/DepartmentsGate";
import { type Facility } from "./hooks/useFacilities";
import { type Service } from "./hooks/useServices";

export interface ServiceQuery {
  service: Service | null;
  sortOrder: string;
  searchText: string;
  date?: string; // ISO yyyy-mm-dd
  time?: string; // HH:mm
  departmentId?: string;
}

function App() {
  const [serviceQuery, setServiceQuery] = useState<ServiceQuery>(
    {} as ServiceQuery
  );
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [lastName, setLastName] = useState("");
  const [firstName, setFirstName] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [facility, setFacility] = useState<Facility | null>(null);
  const [depsAcknowledged, setDepsAcknowledged] = useState(false);
  const [selectedDepartmentId, setSelectedDepartmentId] = useState<string | undefined>(undefined);
  const [recordCode, setRecordCode] = useState<string | null>(null);

  const today = useMemo(() => new Date(), []);
  const daysToShow = 14;

  const toISODate = (d: Date) => {
    const year = d.getFullYear();
    const month = String(d.getMonth() + 1).padStart(2, "0");
    const day = String(d.getDate()).padStart(2, "0");
    return `${year}-${month}-${day}`;
  };

  const addDays = (base: Date, n: number) => {
    const d = new Date(base);
    d.setDate(d.getDate() + n);
    return d;
  };

  const formatDayLabel = (d: Date) => {
    const day = String(d.getDate()).padStart(2, "0");
    const month = String(d.getMonth() + 1).padStart(2, "0");
    return `${day}.${month}`;
  };

  // По умолчанию начинаем выбор с завтрашнего дня
  const selectedDateISO = serviceQuery.date ?? toISODate(addDays(today, 1));
  const selectedIndex = useMemo(() => {
    const sd = new Date(selectedDateISO);
    // Индексация относительно завтрашнего дня
    const base = new Date(toISODate(addDays(today, 1)));
    const diffMs = sd.setHours(0,0,0,0) - base.setHours(0,0,0,0);
    const diffDays = Math.round(diffMs / (1000 * 60 * 60 * 24));
    return Math.min(Math.max(diffDays, 0), daysToShow - 1);
  }, [selectedDateISO, today]);

  const setByIndex = (idx: number) => {
    const clamped = Math.min(Math.max(idx, 0), daysToShow - 1);
    const start = new Date(toISODate(addDays(today, 1))); // старт с завтра
    const newDate = addDays(start, clamped);
    setServiceQuery({ ...serviceQuery, date: toISODate(newDate) });
  };

  if (!facility) {
    return <FacilityPicker onSelect={(f) => setFacility(f)} />;
  }

  if (!depsAcknowledged) {
    return (
      <DepartmentsGate
        facilityId={facility.id}
        onLoaded={(deps) => {
          if (deps && deps.length && !selectedDepartmentId) setSelectedDepartmentId(deps[0].id);
        }}
        onContinue={(depId) => {
          setSelectedDepartmentId(depId);
          setServiceQuery({ ...serviceQuery, departmentId: depId });
          setDepsAcknowledged(true);
        }}
      />
    );
  }

  return (
    <Grid
      templateAreas={{
        base: `"nav" "main"`,
        lg: `"nav nav" "aside main"`,
      }}
      templateColumns={{
        base: "1fr",
        lg: "740px 1fr",
      }}
    >
      <GridItem area="aside" paddingX={5} hideBelow="lg">
        <ServiceList
          serviceQuery={serviceQuery}
          selectedService={serviceQuery.service}
          onSelectService={(service) => {
            // Устанавливаем дату по умолчанию (завтра), если она еще не установлена
            const newDate = serviceQuery.date || toISODate(addDays(today, 1));
            setServiceQuery({ ...serviceQuery, service, date: newDate });
          }}
        />
      </GridItem>
      <GridItem area="main">
        <Box paddingLeft={2} paddingTop={4} paddingRight={4}>
          <VStack align="stretch" gap={3}>
            <HStack justify="space-between" gap={4} wrap="wrap">
              <Heading size="lg">Выбор даты и времени</Heading>
              <Text fontWeight="medium" color="black">{formatDayLabel(new Date(selectedDateISO))}</Text>
            </HStack>
            <HStack gap={3} align="center">
              <IconButton
                aria-label="Предыдущий день"
                onClick={() => setByIndex(selectedIndex - 1)}
                disabled={selectedIndex <= 0}
                size="sm"
                variant="subtle"
              >
                ‹
              </IconButton>
              <Box flex={1}>
                <Slider.Root
                  min={0}
                  max={daysToShow - 1}
                  step={1}
                  value={[selectedIndex]}
                  onValueChange={(details: { value: number[] }) => setByIndex(details.value[0])}
                >
                  <Slider.Track>
                    <Slider.Range />
                  </Slider.Track>
                  <Slider.Thumb index={0} />
                </Slider.Root>
                <HStack justify="space-between" mt={1}>
                  {Array.from({ length: daysToShow }, (_, i) => (
                    <Text key={i} fontSize="xs" color={i === selectedIndex ? "black" : "fg.muted"}>
                      {formatDayLabel(addDays(new Date(toISODate(addDays(today, 1))), i))}
                    </Text>
                  ))}
                </HStack>
              </Box>
              <IconButton
                aria-label="Следующий день"
                onClick={() => setByIndex(selectedIndex + 1)}
                disabled={selectedIndex >= daysToShow - 1}
                size="sm"
                variant="subtle"
              >
                ›
              </IconButton>
            </HStack>
          </VStack>
        </Box>
        <TimeGrid
          serviceQuery={serviceQuery}
          selectedTime={serviceQuery.time}
          onSelectTime={(time: string) => setServiceQuery({ ...serviceQuery, time })}
        />
        <Box paddingX={4} paddingBottom={6} paddingTop={2} display="flex" justifyContent="flex-end">
          <Button
            variant="solid"
            onClick={() => setIsDialogOpen(true)}
            disabled={!selectedDateISO || !serviceQuery.time}
          >
            Записаться
          </Button>
        </Box>

        <Dialog.Root
          open={isDialogOpen}
          onOpenChange={(e) => {
            setIsDialogOpen(e.open);
            if (!e.open) {
              setRecordCode(null);
              setFirstName("");
              setLastName("");
            }
          }}
        >
          <Dialog.Backdrop />
          <Dialog.Positioner>
          <Dialog.Content maxW="md" w="90vw" borderRadius="lg" boxShadow="lg">
            <Dialog.Header>
              <Heading size="md" color="fg" _dark={{ color: "white" }}>Данные для записи</Heading>
            </Dialog.Header>
            <Dialog.Body>
              {recordCode ? (
                <VStack align="stretch" gap={3}>
                  <Heading size="md" color="fg" _dark={{ color: "white" }}>Запись создана</Heading>
                  <Text color="fg" _dark={{ color: "white" }}>Ваш код записи:</Text>
                  <Box
                    borderWidth="1px"
                    borderRadius="md"
                    padding={3}
                    fontSize="xl"
                    fontWeight="bold"
                    textAlign="center"
                    bg="bg.subtle"
                    color="fg"
                    _dark={{ color: "white" }}
                  >
                    {recordCode}
                  </Box>
                  <Box color="fg.muted" fontSize="sm" _dark={{ color: "whiteAlpha.800" }}>
                    <Text>Дата: {selectedDateISO}</Text>
                    <Text>Время: {serviceQuery.time}</Text>
                  </Box>
                </VStack>
              ) : (
                <VStack align="stretch" gap={4}>
                  <Box>
                    <Text mb={1} color="fg" _dark={{ color: "white" }}>Фамилия</Text>
                    <Input
                      value={lastName}
                      color="fg"
                      _dark={{ color: "white" }}
                      onChange={(e) => setLastName(e.target.value)}
                      placeholder="Введите фамилию"
                      size="md"
                      autoFocus
                    />
                  </Box>
                  <Box>
                    <Text mb={1} color="fg" _dark={{ color: "white" }}>Имя</Text>
                    <Input
                      value={firstName}
                      color="fg"
                      _dark={{ color: "white" }}
                      onChange={(e) => setFirstName(e.target.value)}
                      placeholder="Введите имя"
                      size="md"
                    />
                  </Box>
                  <Box color="fg.muted" fontSize="sm" _dark={{ color: "whiteAlpha.800" }}>
                    <Text>Дата: {selectedDateISO}</Text>
                    <Text>Время: {serviceQuery.time}</Text>
                  </Box>
                </VStack>
              )}
            </Dialog.Body>
            <Dialog.Footer>
              <HStack justify="flex-end" w="full">
                {recordCode ? (
                  <Button variant="solid" onClick={() => setIsDialogOpen(false)}>
                    Закрыть
                  </Button>
                ) : (
                  <>
                    <Button variant="subtle" onClick={() => setIsDialogOpen(false)}>Отмена</Button>
                    <Button
                      variant="solid"
                      onClick={async () => {
                        if (!selectedDateISO || !serviceQuery.time || !serviceQuery.service?.serviceId) return;
                        setIsSubmitting(true);
                        try {
                          // Возвращаем отправку локального времени без смещения
                          const recordTime = `${selectedDateISO}T${serviceQuery.time}:00`;

                          
                          const payload = {
                            accountId: "string", // заменить на реальный идентификатор аккаунта, если он доступен
                            serviceId: serviceQuery.service.serviceId,
                            recordTime,
                            name: firstName,
                            surname: lastName,
                            categoryPrefix: serviceQuery.service.categoryPrefix,
                            serviceName: serviceQuery.service.serviceName
                          };
                          console.log('Полный payload:', payload);
                          const res = await apiClient.post("https://localhost:44345/record", payload, {
                            headers: { "Content-Type": "application/json" },
                          });
                          const code = (res?.data as any)?.recordCode ?? null;
                          setRecordCode(code);
                        } catch (e) {
                          console.error("Ошибка отправки записи", e);
                        } finally {
                          setIsSubmitting(false);
                        }
                      }}
                      disabled={!lastName || !firstName || isSubmitting}
                    >
                      {isSubmitting ? "Отправка..." : "Подтвердить"}
                    </Button>
                  </>
                )}
              </HStack>
            </Dialog.Footer>
            <Dialog.CloseTrigger />
          </Dialog.Content>
          </Dialog.Positioner>
        </Dialog.Root>
      </GridItem>
    </Grid>
  );
}

export default App;
