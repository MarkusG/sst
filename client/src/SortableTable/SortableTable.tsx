import { createContext, PropsWithChildren, useState } from "react";

interface SortableTableProps {
    className?: string,
    options: SortOptions,
    onSortUpdated: (options: SortOptions) => void
}

export interface SortOptions {
    field?: string,
    direction?: "up" | "down",
}

interface SortContext {
    options: SortOptions,
    setOptions: (options: SortOptions) => Promise<void>
}

export const TableContext = createContext<SortContext | null>(null);

export default function SortableTable(props: PropsWithChildren<SortableTableProps>) {
    const [options, setOptions] = useState<SortOptions>(props.options);

    async function onSetOptions(options: SortOptions) {
        setOptions(options);
        props.onSortUpdated(options);
    }

    return (
        <TableContext.Provider value={{options, setOptions: onSetOptions}}>
            <table className={props.className}>
                {props.children}
            </table>
        </TableContext.Provider>
    );
}
