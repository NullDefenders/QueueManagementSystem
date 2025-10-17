"use client";

import { useState } from "react";

import { Header } from "./Components/Header/Header";
import { OptionButton } from "./Components/Options/OptionButton/OptionButton";
import { Ticket } from "./Components/Ticket/Ticket";
import { IDepartment, IService, Steps, ITicket } from "./types";

import styles from "./page.module.scss";
import { Options } from "./Components/Options/Options";
import { StepCategories } from "./Components/steps/StepCategories/StepCategories";

const departments: IDepartment[] = [
  {
    id: "01fd2618-6124-4e6b-a140-7130706e2149",
    name: `МФЦ "Верх-Исетский"`,
    address: "г. Екатеринбург, ул. Хомякова, д. 14",
  },
  {
    id: "2790ec1b-d8e2-4923-9a05-4a14bbf48c3e",
    name: `МФЦ "Фрунзенский"`,
    address: "г. Санкт-Петербург, Бухарестская ул., д. 48, оф. 101",
  },
  {
    id: "3838ae60-f14d-48bf-8115-2e9d37d2fe8d",
    name: `МФЦ "Орджоникидзевский"`,
    address: "г. Екатеринбург, ул. Бакинских Комиссаров, д. 53",
  },
  {
    id: "4fa8236c-801a-45e6-a358-352ed56a88b5",
    name: `МФЦ "Северный"`,
    address: "г. Москва, Ленинградский проспект, д. 62, оф. 104",
  },
  {
    id: "50feae97-695f-4779-ad88-5357d1dc2eb8",
    name: `МФЦ "Кировский"`,
    address: "г. Екатеринбург, ул. Декабристов, д. 16",
  },
  {
    id: "5570b277-fa5d-4267-80d5-1dcf47cfc18b",
    name: `МФЦ "Центральный"`,
    address: "г. Москва, ул. Тверская, д. 15, стр. 1",
  },
  {
    id: "676885cb-ee0f-40b4-be03-9d25a8a2dca1",
    name: `МФЦ "Приморский"`,
    address: "г. Санкт-Петербург, ул. Савушкина, д. 83, корп. 3",
  },
  {
    id: "6899f396-3b0b-45cb-b321-e82ec77b15ad",
    name: `МФЦ "Чкаловский"`,
    address: "г. Екатеринбург, ул. Белинского, д. 222",
  },
  {
    id: "764b87fb-1335-43b6-b753-40c01f3b7bb6",
    name: `МФЦ "Калининский"`,
    address: "г. Санкт-Петербург, Гражданский проспект, д. 104, корп. 2",
  },
  {
    id: "775b5d0e-e161-4d6a-8137-74fc1c38e189",
    name: `МФЦ "Красногвардейский"`,
    address: "г. Санкт-Петербург, ул. Ленская, д. 12, лит. Б",
  },
  {
    id: "84fdf6d5-4e0b-4077-bb7c-702782da545d",
    name: `МФЦ "Западный"`,
    address: "г. Москва, ул. Можайский Вал, д. 10",
  },
  {
    id: "97237109-8e2e-44db-85c8-7ebae2faf6bc",
    name: `МФЦ "Восточный"`,
    address: "г. Москва, ул. Первомайская, д. 33, стр. 2",
  },
  {
    id: "9978d1c5-75a4-4082-9eca-50d6b1815394",
    name: `МФЦ "Южный"`,
    address: "г. Москва, Варшавское шоссе, д. 87, корп. 2",
  },
  {
    id: "ab779ac4-a872-4c9a-accd-a1ee340a930a",
    name: `МФЦ "Центральный"`,
    address: "г. Екатеринбург, ул. Ленина, д. 24а",
  },
  {
    id: "d4f39531-e7f0-42f8-8168-2ffa417b0dde",
    name: `МФЦ "Центральный"`,
    address: "г. Санкт-Петербург, Невский проспект, д. 55, лит. А",
  },
];

export default function Home() {
  const [step, setStep] = useState(Steps.Departments);
  const [categories, setCategories] = useState<string[]>([]);
  const [services, setServices] = useState<IService[]>([]);
  const [serviceNames, setServiceNames] = useState<string[]>([]);
  const [ticket, setTicket] = useState<ITicket>();

  const onDepartmentClick = (id: string) => {
    fetch(`http://localhost:5259/api/department/services/${id}`)
      .then((response) => response.json())
      .then((data) => {
        setServices(data.services);
        const newCategories: string[] = [];

        data.services.forEach((service: IService) => {
          if (!newCategories.includes(service.categoryName))
            newCategories.push(service.categoryName);
        });

        setServices(data.services);
        setCategories(newCategories);
        setStep(Steps.Categories);
      });
  };

  const onCategoryClick = (category: string) => {
    const filteredServices: IService[] = services.filter(
      (service) => service.categoryName === category
    );

    const filteredServiceNames = filteredServices.map(
      (service) => service.serviceName
    );

    setServiceNames(filteredServiceNames);
    setStep(Steps.Services);
  };

  const onServcieNameClick = (serviceName: string) => {
    fetch("http://localhost:5120/api/Ticket/GetTicket")
      .then((response) => response.json())
      .then((data: ITicket) => {
        const ticket = { ...data, serviceName: serviceName };
        setTicket(ticket);
        setStep(Steps.Ticket);
      });
  };

  return (
    <div className={styles.page}>
      <Header setStep={setStep} />
      <main className={styles.main}>
        {step === Steps.Departments && (
          <ul className={styles.departmentList}>
            {departments.map((dep, index) => {
              return (
                <OptionButton
                  key={`${dep.name}-${index}`}
                  onClick={() => onDepartmentClick(dep.id)}
                  title={dep.name}
                  desc={dep.address}
                />
              );
            })}
          </ul>
        )}
        {step === Steps.Categories && (
          <StepCategories
            categories={categories}
            onCategoryClick={onCategoryClick}
          />
        )}
        {step === Steps.Services && (
          <Options list={serviceNames} onClick={onServcieNameClick} />
        )}
        {step === Steps.Ticket && ticket && (
          <Ticket
            serviceName={ticket.serviceName}
            talonNumber={ticket.talonNumber}
            issuedAt={ticket.issuedAt}
          />
        )}
      </main>
      <footer className={styles.footer}></footer>
    </div>
  );
}
