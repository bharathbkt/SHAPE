import React, { createContext, useState, useContext } from 'react';

interface AnalysisData {
  ingredients: string[];
  nutritionInfo: any;
}

interface AnalysisContextData {
  analysisData: AnalysisData | null;
  setAnalysisData: (data: AnalysisData) => void;
  clearAnalysis: () => void;
}

const AnalysisContext = createContext<AnalysisContextData>({} as AnalysisContextData);

export const AnalysisProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [analysisData, setAnalysisData] = useState<AnalysisData | null>(null);

  const clearAnalysis = () => {
    setAnalysisData(null);
  };

  return (
    <AnalysisContext.Provider value={{
      analysisData,
      setAnalysisData,
      clearAnalysis,
    }}>
      {children}
    </AnalysisContext.Provider>
  );
};

export const useAnalysis = () => useContext(AnalysisContext);
