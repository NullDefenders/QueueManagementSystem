import { Box, Flex, Grid, GridItem, HStack, Show } from "@chakra-ui/react";
import { useState } from "react";
import TimeGrid from "./components/TimeGrid";
import "./App.css";
import ServiceList from "./components/ServiceList";
import { type Service } from "./hooks/useServices";

export interface ServiceQuery {
  service: Service | null;
  sortOrder: string;
  searchText: string;
}

function App() {
  const [serviceQuery, setServiceQuery] = useState<ServiceQuery>(
    {} as ServiceQuery
  );

  return (
    <Grid
      templateAreas={{
        base: `"nav" "main"`,
        lg: `"nav nav" "aside main"`,
      }}
      templateColumns={{
        base: "1fr",
        lg: "250px 1fr",
      }}
    >
      <GridItem area="aside" paddingX={5} hideBelow="lg">
        <ServiceList
          serviceQuery={serviceQuery}
          selectedService={serviceQuery.service}
          onSelectService={(service) =>
            setServiceQuery({ ...serviceQuery, service })
          }
        />
      </GridItem>
      <GridItem area="main">
        <Box paddingLeft={2}>
          <h1>Время</h1>
        </Box>
        <TimeGrid serviceQuery={serviceQuery} />
      </GridItem>
    </Grid>
  );
}

export default App;
