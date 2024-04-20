import React, { createContext, useContext, useState, ReactNode } from "react";

interface StoreContextProps {
    currentStoreId: string | null;
    setCurrentStoreId: (storeId: string) => void;
    stores: Record<string, string>;
    getCurrentStoreName: () => string;
    setStoresData: (stores: Record<string, string>) => void;
}

const StoreContext = createContext<StoreContextProps | undefined>(undefined);

interface StoreProviderProps {
    children: ReactNode;
}

export const StoreProvider: React.FC<StoreProviderProps> = ({ children }) => {
    const [currentStoreId, setCurrentStoreId] = useState<string | null>(null);
    const [stores, setStores] = useState<Record<string, string>>({});

    const setStoresData = (stores: Record<string, string>) => {
        setStores(stores);
    }

    const getCurrentStoreName = (): string => {
        if (currentStoreId != null) {
            var store = stores![currentStoreId]
            if (store != null) {
                return store;
            }
        }
        return '';
    }

    return (
        <StoreContext.Provider value={{ currentStoreId, setCurrentStoreId, stores,  getCurrentStoreName,setStoresData }}>
            {children}
        </StoreContext.Provider>
    );
};

export const useStore = (): StoreContextProps => {
    const context = useContext(StoreContext);
    if (!context) {
        throw new Error("useStore must be used within a MenuProvider");
    }
    return context;
};