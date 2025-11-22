import { createContext, PropsWithChildren, useState } from "react";

export interface ImportContext {
  files: FileList | null;
  accountId: number | null;
  done: boolean;
}

export const DefaultImportContext = {
  files: null,
  accountId: null,
  done: false,
};

export const ImportContext = createContext<
  [ImportContext, React.Dispatch<React.SetStateAction<ImportContext>>]
>(null!);

export function ImportContextProvider({ children }: PropsWithChildren) {
  const state = useState<ImportContext>(DefaultImportContext);

  return (
    <ImportContext.Provider value={state}>{children}</ImportContext.Provider>
  );
}
