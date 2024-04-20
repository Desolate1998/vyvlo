import { useToastController, Toast, ToastTitle, ToastIntent } from "@fluentui/react-components";
import React, { createContext, useContext,  ReactNode } from "react";


interface ToasterContextProps {
    mainToast:string;
    notify :(message:string,intent:ToastIntent) => void
}

const ToasterContext = createContext<ToasterContextProps | undefined>(undefined);

interface MenuProviderProps {
    children: ReactNode;
}

export const ToasterProvider: React.FC<MenuProviderProps> = ({ children }) => {
    const mainToast ="MainToaster";

    const { dispatchToast } = useToastController(mainToast);
    const notify = (message:string,intent:ToastIntent) =>
      dispatchToast(
        <Toast>
          <ToastTitle>{message}</ToastTitle>
        </Toast>,
        { intent: intent }
      );
    return (
        <ToasterContext.Provider value={{mainToast,notify}}>
            {children}
        </ToasterContext.Provider>
    );
};

export const useToaster = (): ToasterContextProps => {
    const context = useContext(ToasterContext);
    if (!context) {
        throw new Error("useToaster must be used within a MenuProvider");
    }
    return context;
};