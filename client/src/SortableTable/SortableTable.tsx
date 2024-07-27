import { createContext, PropsWithChildren, useState } from "react";

interface SortableTableProps {
    className?: string
}

interface SortOptions {
    field: string | null,
    direction: "up" | "down" | null,
}

interface SortContext {
    options: SortOptions,
    setOptions: (options: SortOptions) => Promise<void>
}

export const TableContext = createContext<SortContext | null>(null);

function SortableTable(props: PropsWithChildren<SortableTableProps>) {
    const [options, setOptions] = useState<SortOptions>({ field: null, direction: null });

    async function onSetOptions(options: SortOptions) {
        setOptions(options);
    }

    return (
        <TableContext.Provider value={{options, setOptions: onSetOptions}}>
            <table className={props.className}>
                {props.children}
            </table>
        </TableContext.Provider>
    );
}

export default SortableTable;
