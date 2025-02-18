import {createContext, PropsWithChildren, useState} from "react";

export enum Focus {
    Amount,
    Category
}

export interface TransactionTableContext {
    transactionId?: number,
    categorizationId?: number,
    focus?: Focus,
    addCategoryAfterIdx?: number
}

export const TransactionTableContext = createContext<[TransactionTableContext, React.Dispatch<React.SetStateAction<TransactionTableContext>>]>(null!);

export default function TransactionTableContextProvider({children}: PropsWithChildren) {
    const state = useState<TransactionTableContext>({});

    return (
        <TransactionTableContext.Provider value={state}>
            {children}
        </TransactionTableContext.Provider>
    );
}